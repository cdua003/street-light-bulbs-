/// <reference path="../Definitions/knockout.d.ts" />
/// <reference path="../Definitions/jquery.d.ts" />

module Rsl {
    export class ApplicationViewModel {
        public streetlights: KnockoutObservable<Models.IStreetlightSummary[]>;
        public selectedStreetlight: KnockoutObservable<IStreetlightDetailViewModel>;
        public loading: KnockoutObservable<boolean>;
        // get applicant to add a loader here
        constructor(private _apiAccess: IApiAccess) {
            this.streetlights = ko.observable<Models.IStreetlightSummary[]>();
            this.selectedStreetlight = ko.observable<IStreetlightDetailViewModel>();
            this.loading = ko.observable(false);
            this.loadData().done(x => {
                this.streetlights(x);
                // Set the first street as default selected street
                if (this.canSetDefaultStreetLight(x)) {
                    this.selectStreetlight(this, x[0]);
                }
            });
        }
        public selectStreetlight(parent: ApplicationViewModel, streetlight: Models.IStreetlightSummary): void {
            parent.selectedStreetlight(null);
            parent.loadDetails(streetlight.id).done(x => {
                parent.selectedStreetlight(x);
            });
            parent.loading(false);
        }

        public clickObject(parent: any, data: any): void {
            parent.set(data);
        }
        public isFailed(bulb: IBulbStateViewModel): boolean {
            return bulb.bulbStatus().faultCondition > 0;
        }

        public toggleLightState(light: IStreetlightDetailViewModel): void {
            this.loading(true);
            var isOn = light.isSwitchedOn();

            if (isOn) {
                this._apiAccess.switchOffLight(light.id).always(x => {
                    this.selectStreetlight(this, {
                        id: light.id,
                        description: light.description
                    });
                });
            }
            else {
                this._apiAccess.switchOnLight(light.id).always(x => {
                    this.selectStreetlight(this, {
                        id: light.id,
                        description: light.description
                    });
                });
            }
        }

        public toggleBulbState(parent: ApplicationViewModel, bulb: IBulbStateViewModel): void {
            parent.loading(true);
            var isOn = bulb.bulbStatus().isOn;

            if (isOn) {
                // always switch off
                parent._apiAccess.switchOffBulb(bulb.bulbInformation.id)
                    .done(x => {
                        // reload bulb data
                        parent.updateBulbStatus(bulb);
                    });
            }
            else {
                // always switch on
                parent._apiAccess.switchOnBulb(bulb.bulbInformation.id)
                    .done(x => {
                        // reload bulb data
                        parent.updateBulbStatus(bulb);
                    });
            }
        }

        private bulbTemperatureStatus(bulb: IBulbStateViewModel): string {
            //the temperature is 0
            if (bulb.bulbStatus().bulbTemperature === 0) {
                return Enums.TemperatureStatus.Normal.toString();
            }
            //the temperature exceeds the max temperature 
            if (bulb.bulbStatus().bulbTemperature > bulb.bulbInformation.maxTemperature) {
                return Enums.TemperatureStatus.Over.toString();
            }
            //the temperature is less than ½ max temperature
            if (bulb.bulbStatus().bulbTemperature < (bulb.bulbInformation.maxTemperature / 2)) {
                return Enums.TemperatureStatus.Low.toString();
            }
            //the temperature is more than or equal to ½ the max temperature and less than or equal to the max temperature
            return Enums.TemperatureStatus.High.toString();
        }

        private getCurrentState(bulb: IBulbStateViewModel): string {
            if (!this.isFailed(bulb)) {
                return 'None';
            }
            if (bulb.bulbStatus().faultCondition === Enums.FaultCode.OverTemperature) {
                return 'Over Temperature';
            }
            if (bulb.bulbStatus().faultCondition === Enums.FaultCode.OverVoltage) {
                return 'Over Voltage';
            }
            if (bulb.bulbStatus().faultCondition === Enums.FaultCode.GeneralFailure) {
                return 'General Failure';
            }
            return 'Repair';
        }

        private toggleStreetlightStatus(streetlight: Models.IStreetlightSummary): string {
            if (this.selectedStreetlight() === undefined || this.selectedStreetlight() === null || this.selectedStreetlight().id !== streetlight.id) {
                return '';
            }
            return 'active';
        }

        private totalPowerDraw(light: IStreetlightDetailViewModel): string {
            //Only consider when streetlight is ON
            if (light === undefined || light === null || !light.isSwitchedOn()) {
                return '0';
            }
            let powerDraw = 0;
            for (let i = 0, max = light.bulbs.length; i < max; i++) {
                let bulb = light.bulbs[i];
                //Only bulbs that are switched on should be included.
                if (!bulb.bulbStatus().isOn || this.isFailed(bulb)) {
                    continue;
                }
                powerDraw += bulb.bulbInformation.powerDraw;
            }
            return powerDraw.toString();
        }

        private setOrRepair(parent: ApplicationViewModel, bulb: IBulbStateViewModel): void {
            parent.loading(true); // start loading

            // get next fault state
            let fault = !parent.isFailed(bulb) ? Enums.FaultCode.OverTemperature :
                bulb.bulbStatus().faultCondition === Enums.FaultCode.GeneralFailure ? Enums.FaultCode.None : (bulb.bulbStatus().faultCondition + 1);

            // start updating
            parent.setfault(bulb.bulbInformation.id, fault).done(x => {

                // reload bulb data
                parent.processBulbStatus(x, parent, bulb);
            });
        }

        private processBulbStatus(status: boolean, parent: ApplicationViewModel, bulb: IBulbStateViewModel): void {
            if (status) {
                // reload bulb data
                parent.updateBulbStatus(bulb);
            } else {
                // stop loading if updating failed
                parent.loading(false);
            }
        }

        private updateBulbStatus(bulb: IBulbStateViewModel) {
            this._apiAccess.loadBulbDetail(bulb.bulbInformation.id).done(x => {
                bulb.bulbStatus(x.bulbCurrentState);
            });
            this.loading(false);
        }

        private canSetDefaultStreetLight(streetLights: Models.IStreetlightSummary[]): boolean {
            return streetLights !== undefined && streetLights !== null && streetLights.length > 0;
        }

        private loadData(): JQueryPromise<Models.IStreetlightSummary[]> {
            return this._apiAccess.loadStreetlights();
        }

        private loadDetails(id: string): JQueryPromise<IStreetlightDetailViewModel> {
            return this._apiAccess.loadStreetlightDetail(id);
        }

        // to change bulb fault state
        private setfault(id: string, fault: Enums.FaultCode): JQueryPromise<boolean> {
            return this._apiAccess.setfault(id, fault);
        }
    }
}