/* jshint esversion: 6 */
var PacketFactory = require("./PacketFactory.js").PacketFactory;
var Game = require("./Game.js").Game;
var Player = require("./Player.js").Player;

// server class
exports.Server = class Server {
    constructor() {
        this.abc = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        this.socket = require('dgram').createSocket('udp4');
        this.port = 5643;
        this.players = [];
        this.games = [];
        this.socket.on('message', (msg, rinfo) => { this.onPacket(msg, rinfo); });
        this.socket.on('close', () => { this.onClose(); });
        this.socket.bind(this.port, () => {
            console.log("Server listening on port " + this.port);
            this.socket.setBroadcast(true);
            //this.loop();
         });
    }

////////////////////// socket management    
    onClose() {
        console.log("A user has disconnected.");
    }
    onPacket(packet, rinfo) {
        if(packet.length < 4) return;
        let json = JSON.parse(packet);

        switch(json.type) {
            case "JREQ":
                this.readJoinRequest(json, rinfo);
                break;
            case "FGAM":
                this.readFindGame(json, rinfo);
                break;
            case "NGAM":
                this.readNewGame(json, rinfo);
                break;
            case "ENDT":
                this.readEndTurn(json, rinfo);
                break;
            case "JGAM":
                this.readJoinGame(json, rinfo);
                break;
            case "LEAV":
                this.readLeaveGame(json, rinfo);
                break;
            case "GMOV":
                this.readGameOver(json, rinfo);
                break;
        }
    }
    loop() {
        setTimeout(() => { this.loop(); }, 1000);
    }
    send(buffer, rinfo) {
        this.socket.send(buffer, 0, buffer.length, rinfo.port, rinfo.address);
    }
////////////////////// Game management
    createOpenGame(mapID) {
        return this.createGame(false, mapID);
    }
    createPrivateGame(mapID) {
        return this.createGame(true, mapID);
    }
    createGame(flag, mapID) {
        let gameKey = this.createGameKey(); // get a random key here
        let newGame = new Game(this, gameKey, flag, mapID);
        this.games.push(newGame);
        return newGame;
    }
    createGameKey() 
    {
        let key = "";
        for(let i = 0; i < 4; i++) {
            let index = Math.floor((Math.random() * 26));
            key += this.abc[index];
        }
        if(this.isDuplicateGameKey()) return this.createGameKey();
        return key;
    }
    isDuplicateGameKey(key) {
        this.games.map((game) => {
            if(key == game.key) return true;
        });
        return false;
    }
    findOpenGame() {
        let gameToReturn = null;
        this.games.map((game) => {
            if(game.players.length == 1 && !game.isPrivate) {
                console.log("Open Game Found!");
                gameToReturn = game;
            }
        });
        if(gameToReturn == null) console.log("No Open Games Available");
        return gameToReturn;
    }
    lookUpGame(key) {
        let gameToReturn = null;
        this.games.map((game) => {
            if(game.key == key) {
                console.log("Game found: " + key);
                gameToReturn = game;
            }
        });
        if(gameToReturn == null) console.log("No game with key "+key+" found");
        return gameToReturn;
    }
    removeGame(game) {
        if(game != null) {
            this.games.splice(this.games.indexOf(game), 1);
            console.log("Game removed from server");
            console.log("Game Count: " + this.games.length);
        } else {
            console.log("Game not removed");
        }
    }
////////////////////// packet readers
    readJoinRequest(packet, rinfo) {
        //console.log("Join Request Recieved.");
        let buff = PacketFactory.buildJoinResponse(1);
        this.send(buff, rinfo);
    }
    readFindGame(packet, rinfo) {
        //console.log("Find Game Recevied");
        let game = this.findOpenGame();
        if(game == null) { return; }
        game.addPlayer(rinfo);
        let buff = PacketFactory.buildStartGame(game.mapID, game.key);
        game.broadcast(buff);
    }
    readNewGame(packet, rinfo) {
        //console.log("New Game Recieved");
        let newGame = this.createOpenGame(packet.mapID);
        newGame.addPlayer(rinfo);
        let buff = PacketFactory.buildGameKey(newGame.key);
        this.send(buff, rinfo);
    }
    readEndTurn(packet, rinfo) {
        //console.log("End Turn Recieved");
        let game = this.lookUpGame(packet.gameKey);
        if(game == null) return;
        game.players.map((player) => {
            if(!player.matches(rinfo)) {
                console.log("End Turn Sent");
                this.send(PacketFactory.buildEndTurn(packet), player.rinfo);
            }
        });
    }
    readJoinGame(packet, rinfo) {
        let game = this.lookUpGame(packet.gameKey);
        if(game == null) return; // should send back an error here
        if(game.players.length > 2) return;
        if(game.players.length == 0) {
            this.removeGame(game.key);
            return;
        }
        game.addPlayer(rinfo);
        let buff = PacketFactory.buildStartGame(game.mapID, game.key);
        game.broadcast(buff);
    }
    readLeaveGame(packet, rinfo) {
        console.log("Player left the game");
        let game = this.lookUpGame(packet.gameKey);
        if(game == null) return;
        game.removeAllPlayers();
        game.endGame();
        this.removeGame(game);
    }
    readGameOver(packet) {
        console.log("Game Over");
        let game = this.lookUpGame(packet.gameKey);
        game.removeAllPlayers();
        this.removeGame(game);
    }
//////////////////////
}