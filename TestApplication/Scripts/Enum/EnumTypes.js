var Rsl;
(function (Rsl) {
    var Enums;
    (function (Enums) {
        var TemperatureStatus;
        (function (TemperatureStatus) {
            TemperatureStatus["Normal"] = "bulb-temperature-normal";
            TemperatureStatus["Low"] = "bulb-temperature-low";
            TemperatureStatus["High"] = "bulb-temperature-high";
            TemperatureStatus["Over"] = "bulb-temperature-over";
        })(TemperatureStatus = Enums.TemperatureStatus || (Enums.TemperatureStatus = {}));
        var RequestType;
        (function (RequestType) {
            RequestType["Get"] = "GET";
            RequestType["Post"] = "POST";
            RequestType["Delete"] = "DELETE";
        })(RequestType = Enums.RequestType || (Enums.RequestType = {}));
        var FaultCode;
        (function (FaultCode) {
            FaultCode[FaultCode["None"] = 0] = "None";
            FaultCode[FaultCode["OverTemperature"] = 1] = "OverTemperature";
            FaultCode[FaultCode["OverVoltage"] = 2] = "OverVoltage";
            FaultCode[FaultCode["GeneralFailure"] = 3] = "GeneralFailure";
        })(FaultCode = Enums.FaultCode || (Enums.FaultCode = {}));
    })(Enums = Rsl.Enums || (Rsl.Enums = {}));
})(Rsl || (Rsl = {}));
//# sourceMappingURL=EnumTypes.js.map