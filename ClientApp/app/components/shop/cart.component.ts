import { Component } from "@angular/core";
import { DataService } from "../shared/data.service";
import { Router } from "@angular/router";

@Component({
	selector: "the-cart",
	templateUrl: "./cart.component.html",
	styleUrls: []

})
export class CartComponent {
	constructor(private data: DataService, private router: Router) {

	}
}