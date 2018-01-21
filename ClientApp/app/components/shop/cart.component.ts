import { Component } from "@angular/core";
import { DataService } from "../shared/data.service";
import { Router } from "@angular/router";

@Component({
	selector: "the-cart",
	templateUrl: "./cart.component.html",
	styleUrls: []

})
export class CartComponent {

	loginRequired: boolean = true;

	constructor(private data: DataService, private router: Router) {
		this.loginRequired = data.loginRequired;
	}

	if(this.loginRequired) {
		this.router.navigate(["login"]);
	}
	else {
	this.router.navigate(["checkout"]);
	}
}