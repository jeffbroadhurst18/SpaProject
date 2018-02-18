import { Component, OnInit } from "@angular/core";
import { DataService } from "../shared/data.service";
import { Router } from "@angular/router";
import { Location } from '@angular/common';

@Component({
	selector: "login",
	templateUrl: "login.component.html",
	styleUrls: ['./login.component.css']
})
export class LoginComponent {

	constructor(private data: DataService, private router: Router,
				private location:Location) {

	}

	errorMessage: string = "";
	public creds = {
		username: "",
		password: ""
	};

		onLogin() {
		this.data.login(this.creds)
			.subscribe(success => {
				if (success) {
					this.data.userName = this.creds.username;
					//if (this.data.order.items.length == 0) {
					//	this.router.navigate(["history"]); //if no orders then go to root
					//} else {
					//	this.router.navigate(["checkout"]);
					//}
					this.location.back();
				} 
			},
			err => this.errorMessage = "Failed to Login");
			//Two way binding is reflected on the form.
	}

		onLogout() {
			this.data.loginRequired = true;
			this.data.userName = "";
		}

		onCancel() {
			this.router.navigate([""]);
		}
}