
const constants = require('./constants');

// quaternion class for rotation
class Quaternion {
    constructor(a, b, c, d) {
        this.a = a;
        this.b = b;
        this.c = c;
        this.d = d;
    }
}

// position class for keeping track of (x, y, z) coordinates
class Position {
    constructor(altitude, latitude, longitude) {
        this.altitude = altitude;
        this.latitude = latitude;
        this.longitude = longitude;
    }
}

class SystemState {
    constructor(name, position, rotation) {
        this.name = name;
        this.position = position;
        this.rotation = rotation;
    }
}

// keeps track of total state
class TotalState {

    constructor(arr) {

        let systemStates = [];
        
        for (let i = 0; i < constants.systems.length; i++) {
            
            let name = constants.systems[i];

            let posStart = i * 3;
            let altitude = arr[posStart];
            let latitude = arr[posStart + 1];
            let longitude = arr[posStart + 2];
            let pos = new Position(altitude, latitude, longitude);

            // rotations start immediately after positions
            let rotStart = constants.systems.length * 3 + 4 * i;
            let a = arr[rotStart];
            let b = arr[rotStart + 1];
            let c = arr[rotStart + 2];
            let d = arr[rotStart + 3];
            let rot = new Quaternion(a, b, c, d);

            let systemState = new SystemState(name, pos, rot);
            systemStates.push(systemState);

        }

        this.systemStates = systemStates;
        this.engineFlag = arr[arr.length - 2];
        this.reserved = arr[arr.length - 1];

    }

}

module.exports = {
    Quaternion,
    Position,
    SystemState,
    TotalState
};
