/* jshint esversion: 6 */

exports.Player = class Player {
    constructor(rinfo, game) {
        this.rinfo = rinfo;
        this.game = game;
    }
    matches(rinfo){
		if(rinfo.address != this.rinfo.address) return false;
		if(rinfo.port != this.rinfo.port) return false;
		return true;
	}
}