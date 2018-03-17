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
		private location: Location) {

	}

	errorMessage: string = "";
	public creds = {
		username: "",
		password: "",
		role: ""
	};

	onLogin() {
		this.data.login(this.creds)
			.subscribe(success => {
				if (success) {
					this.data.userName = this.creds.username;

					this.data.getRole(this.creds.username)
						.subscribe(success => {
							this.creds.role = this.data.role;
						})

					this.location.back();
				}
			},
			err => {
			this.errorMessage = "Failed to Login";
			});
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