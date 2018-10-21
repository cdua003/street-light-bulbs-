/// <reference path="../Definitions/knockout.d.ts" />
var Rsl;
(function (Rsl) {
    var MY_API = "/api/streetlights/";
    var ApiAccess = /** @class */ (function () {
        function ApiAccess() {
        }
        ApiAccess.prototype.loadStreetlights = function () {
            return this.makeRequest();
        };
        ApiAccess.prototype.loadStreetlightDetail = function (id) {
            var deferred = $.Deferred();
            this.makeRequest([id]).done(function (streetlight) {
                deferred.resolve({
                    description: streetlight.description,
                    electricalDraw: streetlight.electricalDraw,
                    id: streetlight.id,
                    isSwitchedOn: ko.observable(streetlight.isSwitchedOn),
                    bulbs: (streetlight.bulbs ? streetlight.bulbs.map(function (x) {
                        return {
                            bulbInformation: x.bulbInformation,
                            bulbStatus: ko.observable(x.bulbCurrentState)
                        };
                    }) : null)
                });
            })
                .fail(function () {
                deferred.reject();
            });
            return deferred.promise();
        };
        ApiAccess.prototype.loadBulbDetail = function (id) {
            return this.makeRequest(["bulb", id]);
        };
        ApiAccess.prototype.switchOffLight = function (id) {
            return this.makeRequest([id, "off"], Rsl.Enums.RequestType.Post.toString());
        };
        ApiAccess.prototype.switchOnLight = function (id) {
            return this.makeRequest([id, "on"], Rsl.Enums.RequestType.Post.toString());
        };
        ApiAccess.prototype.switchOffBulb = function (id) {
            return this.makeRequest(["bulb", id, "off"], Rsl.Enums.RequestType.Post.toString());
        };
        ApiAccess.prototype.switchOnBulb = function (id) {
            return this.makeRequest(["bulb", id, "on"], Rsl.Enums.RequestType.Post.toString());
        };
        ApiAccess.prototype.setfault = function (id, fault) {
            return this.makeRequest(["setfault", id, fault.toString()], Rsl.Enums.RequestType.Post.toString());
        };
        ApiAccess.prototype.makeRequest = function (pathElements, type, data) {
            if (pathElements === void 0) { pathElements = []; }
            if (type === void 0) { type = Rsl.Enums.RequestType.Get.toString(); }
            return $.ajax({
                url: location.protocol + '//' + location.host + MY_API + pathElements.join("/"),
                data: data ? JSON.stringify(data) : null,
                contentType: "application/json",
                type: type,
                crossDomain: false,
                cache: false
            });
        };
        return ApiAccess;
    }());
    Rsl.ApiAccess = ApiAccess;
})(Rsl || (Rsl = {}));
//# sourceMappingURL=data-access-layer.js.map