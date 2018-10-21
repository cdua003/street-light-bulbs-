module Rsl.Models {
    export interface IStreetlightSummary {
        description: string;
        id: string;
    }

    export interface IStreetlightDetail extends IStreetlightSummary {
        bulbs: IBulbState[];
        isSwitchedOn: boolean;
        electricalDraw: number;
    }

    export interface IBulbState {
        bulbInformation: IBulbInformation;
        bulbCurrentState: IBulbStatus;
    }

    export interface IBulbInformation {
        id: string;
        maxTemperature: number;
        powerDraw: number;
        maxHours: number;
    }

    export interface IBulbStatus {
        bulbTemperature: number;
        bulbHours: number;
        isOn: boolean;
        faultCondition: number;
    }
}