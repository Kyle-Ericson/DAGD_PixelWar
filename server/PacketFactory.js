/* jshint esversion: 6 */
var PacketFactory = require("./PacketFactory.js").PacketFactory;

exports.PacketFactory = {
        buildJoinResponse: (response) => {
            let json = { 
                "type": "JRES",
                "response": response
            }; 
            let jsonPacket = JSON.stringify(json); // make json data a string
            let packet = Buffer.alloc(jsonPacket.length); // alloc buffer size
            packet.write(jsonPacket);
            return packet;
        },
        buildStartGame: (mapID, key) => {
            let json = {
                "type": "GMST",
                "mapID": mapID,
                "gameKey": key
            };
            let jsonPacket = JSON.stringify(json); // make json data a string
            let packet = Buffer.alloc(jsonPacket.length); // alloc buffer size
            packet.write(jsonPacket);
            return packet;
        },
        buildGameKey: (key) => {
            let json = {
                "type": "GKEY",
                "gameKey": key
            };
            let jsonPacket = JSON.stringify(json); // make json data a string
            let packet = Buffer.alloc(jsonPacket.length); // alloc buffer size
            packet.write(jsonPacket);
            return packet;
        },
        buildError: (error) => {
            let json = {
                "type": "EROR",
                "error": error
            };
            let jsonPacket = JSON.stringify(json); // make json data a string
            let packet = Buffer.alloc(jsonPacket.length); // alloc buffer size
            packet.write(jsonPacket);
            return packet;
        },
        buildEndTurn: (json) => {
            let jsonPacket = JSON.stringify(json); // make json data a string
            let packet = Buffer.alloc(jsonPacket.length); // alloc buffer size
            packet.write(jsonPacket);
            return packet;
        },
        buildEndGame: () => {
            let json = {
                "type": "EDGM"
            };
            let jsonPacket = JSON.stringify(json);
            let packet = Buffer.alloc(jsonPacket.length);
            packet.write(jsonPacket);
            return packet;
        }
}