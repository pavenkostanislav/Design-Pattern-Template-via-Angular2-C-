"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var http_1 = require("@angular/http");
var Rx_1 = require("rxjs/Rx");
var GridService = (function () {
    function GridService(http) {
        this.http = http;
    }
    // Grid
    GridService.prototype.getGridList = function (parentId) {
        if (parentId)
            return this.http.get("/api/" + this.controllerName + "/list/" + parentId).map(function (res) { return res.json(); });
        else
            return this.http.get("/api/" + this.controllerName + "/list?" + parentId).map(function (res) { return res.json(); });
    };
    GridService.prototype.getGridRowModel = function (id, mode) {
        return this.http.get("/api/" + this.controllerName + "/" + id + "?mode=" + mode).map(function (res) { return res.json(); });
    };
    GridService.prototype.saveGridRowModel = function (model) {
        var body = JSON.stringify(model);
        var headers = new http_1.Headers({
            'Content-Type': 'application/json'
        });
        return this.http.post("/api/" + this.controllerName, body, { headers: headers }).map(function (res) {
            if (res.status && res.status == 200) {
                return res.json();
            }
            else {
                return false;
            }
        });
    };
    GridService.prototype.deleteGridRowModel = function (id) {
        if (!id) {
            return Rx_1.Observable.of(false);
        }
        return this.http.delete("/api/" + this.controllerName + "/" + id).map(function (res) { return res.ok; });
    };
    return GridService;
}());
GridService = __decorate([
    core_1.Injectable()
], GridService);
exports.GridService = GridService;
