/* jshint esversion: 6 */

var PacketFactory = require("./PacketFactory.js").PacketFactory;
var Player = require("./Player.js").Player;

exports.Game = class Game {
    constructor(server, key, flag, mapID) {
        console.log("New Game Created: " + key);
        this.server = server;
        this.key = key;
        this.mapID = mapID;
        this.players = [];
        this.isPrivate = flag;
        this.host = null;
    }
    broadcast(buffer) {
        this.players.map((player) => {
            this.server.send(buffer, player.rinfo);
        });
    }
    addPlayer(rinfo) {
        let newPlayer = new Player(rinfo, this);
        if(this.players.push(newPlayer) == 1) {
            this.host = this.newPlayer;
        }
    }
    removePlayer(rinfo) {
        let player = this.lookUpPlayer(rinfo);
        this.players.splice(this.players.indexOf(player), 1);
    }
    removeAllPlayers(rinfo) {
        this.players.map((player) => {
            this.removePlayer(player.rinfo);
        });
    }
    lookUpPlayer(rinfo) {
        let result = null;
        this.players.map((player) => {
            if(player.matches(rinfo)) result = player;
        });
        return result;
    }
    endGame() {
        this.broadcast(PacketFactory.buildEndGame());
    }

}