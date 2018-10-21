/// <reference path="../Definitions/knockout.d.ts" />
/// <reference path="../Definitions/jquery.d.ts" />
var Rsl;
(function (Rsl) {
    var ApplicationViewModel = /** @class */ (function () {
        // get applicant to add a loader here
        function ApplicationViewModel(_apiAccess) {
            var _this = this;
            this._apiAccess = _apiAccess;
            this.streetlights = ko.observable();
            this.selectedStreetlight = ko.observable();
            this.loading = ko.observable(false);
            this.loadData().done(function (x) {
                _this.streetlights(x);
                // Set the first street as default selected street
                if (_this.canSetDefaultStreetLight(x)) {
                    _this.selectStreetlight(_this, x[0]);
                }
            });
        }
        ApplicationViewModel.prototype.selectStreetlight = function (parent, streetlight) {
            parent.selectedStreetlight(null);
            parent.loadDetails(streetlight.id).done(function (x) {
                parent.selectedStreetlight(x);
            });
            parent.loading(false);
        };
        ApplicationViewModel.prototype.clickObject = function (parent, data) {
            parent.set(data);
        };
        ApplicationViewModel.prototype.isFailed = function (bulb) {
            return bulb.bulbStatus().faultCondition > 0;
        };
        ApplicationViewModel.prototype.toggleLightState = function (light) {
            var _this = this;
            this.loading(true);
            var isOn = light.isSwitchedOn();
            if (isOn) {
                this._apiAccess.switchOffLight(light.id).always(function (x) {
                    _this.selectStreetlight(_this, {
                        id: light.id,
                        description: light.description
                    });
                });
            }
            else {
                this._apiAccess.switchOnLight(light.id).always(function (x) {
                    _this.selectStreetlight(_this, {
                        id: light.id,
                        description: light.description
                    });
                });
            }
        };
        ApplicationViewModel.prototype.toggleBulbState = function (parent, bulb) {
            parent.loading(true);
            var isOn = bulb.bulbStatus().isOn;
            if (isOn) {
                // always switch off
                parent._apiAccess.switchOffBulb(bulb.bulbInformation.id)
                    .done(function (x) {
                    // reload bulb data
                    parent.updateBulbStatus(bulb);
                });
            }
            else {
                // always switch on
                parent._apiAccess.switchOnBulb(bulb.bulbInformation.id)
                    .done(function (x) {
                    // reload bulb data
                    parent.updateBulbStatus(bulb);
                });
            }
        };
        ApplicationViewModel.prototype.bulbTemperatureStatus = function (bulb) {
            //the temperature is 0
            if (bulb.bulbStatus().bulbTemperature === 0) {
                return Rsl.Enums.TemperatureStatus.Normal.toString();
            }
            //the temperature exceeds the max temperature 
            if (bulb.bulbStatus().bulbTemperature > bulb.bulbInformation.maxTemperature) {
                return Rsl.Enums.TemperatureStatus.Over.toString();
            }
            //the temperature is less than ½ max temperature
            if (bulb.bulbStatus().bulbTemperature < (bulb.bulbInformation.maxTemperature / 2)) {
                return Rsl.Enums.TemperatureStatus.Low.toString();
            }
            //the temperature is more than or equal to ½ the max temperature and less than or equal to the max temperature
            return Rsl.Enums.TemperatureStatus.High.toString();
        };
        ApplicationViewModel.prototype.getCurrentState = function (bulb) {
            if (!this.isFailed(bulb)) {
                return 'None';
            }
            if (bulb.bulbStatus().faultCondition === Rsl.Enums.FaultCode.OverTemperature) {
                return 'Over Temperature';
            }
            if (bulb.bulbStatus().faultCondition === Rsl.Enums.FaultCode.OverVoltage) {
                return 'Over Voltage';
            }
            if (bulb.bulbStatus().faultCondition === Rsl.Enums.FaultCode.GeneralFailure) {
                return 'General Failure';
            }
            return 'Repair';
        };
        ApplicationViewModel.prototype.toggleStreetlightStatus = function (streetlight) {
            if (this.selectedStreetlight() === undefined || this.selectedStreetlight() === null || this.selectedStreetlight().id !== streetlight.id) {
                return '';
            }
            return 'active';
        };
        ApplicationViewModel.prototype.totalPowerDraw = function (light) {
            //Only consider when streetlight is ON
            if (light === undefined || light === null || !light.isSwitchedOn()) {
                return '0';
            }
            var powerDraw = 0;
            for (var i = 0, max = light.bulbs.length; i < max; i++) {
                var bulb = light.bulbs[i];
                //Only bulbs that are switched on should be included.
                if (!bulb.bulbStatus().isOn || this.isFailed(bulb)) {
                    continue;
                }
                powerDraw += bulb.bulbInformation.powerDraw;
            }
            return powerDraw.toString();
        };
        ApplicationViewModel.prototype.setOrRepair = function (parent, bulb) {
            parent.loading(true); // start loading
            // get next fault state
            var fault = !parent.isFailed(bulb) ? Rsl.Enums.FaultCode.OverTemperature :
                bulb.bulbStatus().faultCondition === Rsl.Enums.FaultCode.GeneralFailure ? Rsl.Enums.FaultCode.None : (bulb.bulbStatus().faultCondition + 1);
            // start updating
            parent.setfault(bulb.bulbInformation.id, fault).done(function (x) {
                // reload bulb data
                parent.processBulbStatus(x, parent, bulb);
            });
        };
        ApplicationViewModel.prototype.processBulbStatus = function (status, parent, bulb) {
            if (status) {
                // reload bulb data
                parent.updateBulbStatus(bulb);
            }
            else {
                // stop loading if updating failed
                parent.loading(false);
            }
        };
        ApplicationViewModel.prototype.updateBulbStatus = function (bulb) {
            this._apiAccess.loadBulbDetail(bulb.bulbInformation.id).done(function (x) {
                bulb.bulbStatus(x.bulbCurrentState);
            });
            this.loading(false);
        };
        ApplicationViewModel.prototype.canSetDefaultStreetLight = function (streetLights) {
            return streetLights !== undefined && streetLights !== null && streetLights.length > 0;
        };
        ApplicationViewModel.prototype.loadData = function () {
            return this._apiAccess.loadStreetlights();
        };
        ApplicationViewModel.prototype.loadDetails = function (id) {
            return this._apiAccess.loadStreetlightDetail(id);
        };
        // to change bulb fault state
        ApplicationViewModel.prototype.setfault = function (id, fault) {
            return this._apiAccess.setfault(id, fault);
        };
        return ApplicationViewModel;
    }());
    Rsl.ApplicationViewModel = ApplicationViewModel;
})(Rsl || (Rsl = {}));
//# sourceMappingURL=application-viewmodel.js.map