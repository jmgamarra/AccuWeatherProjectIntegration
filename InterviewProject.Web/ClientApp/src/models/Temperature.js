import UnitValue from './UnitValue';

export class Temperature {
    constructor(minimum, maximum) {
        this.minimum = minimum instanceof UnitValue ? minimum : new UnitValue(minimum.value, minimum.unit);
        this.maximum = maximum instanceof UnitValue ? maximum : new UnitValue(maximum.value, maximum.unit);
    }
}