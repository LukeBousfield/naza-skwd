
module.exports = {

    hexToBinary: num => {
        return (num >>> 0).toString(2).padStart(8, '0');
    },

    bitArrToInt: arr => {
        let val = 0;
        for (let i = 0; i < arr.length; i++) {
            val += Math.pow(2, i) * arr[i];
        }
        return val;
    },

    bitArrToDouble: function(arr) {
        console.log('Number of bits: ' + arr.length);
        arr = arr.reverse();
        let sign = arr[0];
        let exponentBits = arr.slice(1, 12);
        let fractionBits = arr.slice(13);

        let signValue = Math.pow(-1, sign);
        let exponentValue = Math.pow(this.bitArrToInt(exponentBits) - 1023);
        let fractionValue = 1;
        for (let i = 1; i <= 52; i++) {
            let currBit = fractionBits[52 - i];
            fractionValue += currBit * Math.pow(2, -i);
        }

        return signValue * exponentValue * fractionValue;

    }

};
