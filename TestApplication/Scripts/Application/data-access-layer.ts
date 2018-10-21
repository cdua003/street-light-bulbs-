/// <reference path="../Definitions/knockout.d.ts" />

module Rsl {
    const MY_API: string = "/api/streetlights/";

    export interface IStreetlightDetailViewModel {
        description: string;
        id: string;
        bulbs: IBulbStateViewModel[];
        isSwitchedOn: KnockoutObservable<boolean>;
        electricalDraw: number;

    }
    export interface IBulbStateViewModel {
        bulbInformation: Models.IBulbInformation;
        bulbStatus: KnockoutObservable<Models.IBulbStatus>;
    }

    export interface IApiAccess {
        loadStreetlights(): JQueryPromise<Models.IStreetlightSummary[]>;
        loadStreetlightDetail(id: string): JQueryPromise<IStreetlightDetailViewModel>;
        loadBulbDetail(id: string): JQueryPromise<Models.IBulbState>;

        // control functions
        switchOffLight(id: string): JQueryPromise<boolean>;
        switchOnLight(id: string): JQueryPromise<string[]>;

        switchOffBulb(id: string): JQueryPromise<boolean>;
        switchOnBulb(id: string): JQueryPromise<boolean>;

        setfault(id: string, fault: Enums.FaultCode): JQueryPromise<boolean>;
    }

    export class ApiAccess implements IApiAccess {
        constructor() {
        }

        public loadStreetlights(): JQueryPromise<Models.IStreetlightSummary[]> {
            return this.makeRequest();
        }

        public loadStreetlightDetail(id: string): JQueryPromise<IStreetlightDetailViewModel> {
            var deferred = $.Deferred<IStreetlightDetailViewModel>();
            this.makeRequest<Models.IStreetlightDetail>([id]).done(streetlight => {
                deferred.resolve({
                    description: streetlight.description,
                    electricalDraw: streetlight.electricalDraw,
                    id: streetlight.id,
                    isSwitchedOn: ko.observable(streetlight.isSwitchedOn),
                    bulbs: (streetlight.bulbs ? streetlight.bulbs.map<IBulbStateViewModel>(x => {
                        return {
                            bulbInformation: x.bulbInformation,
                            bulbStatus: ko.observable(x.bulbCurrentState)
                        };
                    }) : null)
                });
            })
                .fail(() => {
                    deferred.reject();
                });

            return deferred.promise();
        }

        public loadBulbDetail(id: string): JQueryPromise<Models.IBulbState> {
            return this.makeRequest<Models.IBulbState>(["bulb", id]);
        }

        public switchOffLight(id: string): JQueryPromise<boolean> {
            return this.makeRequest<boolean>([id, "off"], Enums.RequestType.Post.toString());
        }

        public switchOnLight(id: string): JQueryPromise<string[]> {
            return this.makeRequest<string[]>([id, "on"], Enums.RequestType.Post.toString());
        }

        public switchOffBulb(id: string): JQueryPromise<boolean> {
            return this.makeRequest<boolean>(["bulb", id, "off"], Enums.RequestType.Post.toString());
        }

        public switchOnBulb(id: string): JQueryPromise<boolean> {
            return this.makeRequest<boolean>(["bulb", id, "on"], Enums.RequestType.Post.toString());
        }

        public setfault(id: string, fault: Enums.FaultCode): JQueryPromise<boolean> {
            return this.makeRequest<boolean>(["setfault", id, fault.toString()], Enums.RequestType.Post.toString());
        }

        private makeRequest<T>(pathElements: string[] = [], type: string = Enums.RequestType.Get.toString(), data?: any): JQueryPromise<T> {
            return $.ajax({
                url: location.protocol + '//' + location.host + MY_API + pathElements.join("/"),
                data: data ? JSON.stringify(data) : null,
                contentType: "application/json",
                type: type,
                crossDomain: false,
                cache: false
            });
        }
    }
}