
const dgram = require('dgram');

const PORT = 42055;
const MULTICAST_ADDR = '237.7.7.7';

const helpers = require('./helpers');

const socket = dgram.createSocket({
    type: 'udp4',
    reuseAddr: true
});

socket.bind(PORT);

socket.on('listening', () => {
    socket.addMembership(MULTICAST_ADDR);
    const address = socket.address();
    console.log(`UDP socket listening on ${address.address}:${address.port}`);
});

socket.on('message', (message, rinfo) => {
    console.log(`Message from: ${rinfo.address}:${rinfo.port}`);
    console.log(`Message length: ${message.length}`);
    let values = [];
    let currOffset = 0;
    try {
        let num = 0;
        while (true) {
            if (num > 20) {
                values.push(message.readInt8(currOffset));
                currOffset += 4;
            } else {
                values.push(message.readDoubleLE(currOffset));
                currOffset += 8;
            }
            num++;
        }
    } catch (err) {}
    console.log(values);
});
