module Rsl.Enums {
    export enum TemperatureStatus {
        Normal = 'bulb-temperature-normal',
        Low = 'bulb-temperature-low',
        High = 'bulb-temperature-high',
        Over = 'bulb-temperature-over'
    }

    export enum RequestType {
        Get = 'GET',
        Post = 'POST',
        Delete = 'DELETE'
    }

    export enum FaultCode {
        None = 0,
        OverTemperature = 1,
        OverVoltage = 2,
        GeneralFailure = 3,
    }
}