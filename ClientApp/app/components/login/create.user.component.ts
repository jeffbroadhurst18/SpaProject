import { Component, OnInit } from "@angular/core";
import { DataService } from "../shared/data.service";
import { Router } from "@angular/router";
import { Location } from '@angular/common';
import { FormGroup, FormBuilder, FormControl, Validators } from "@angular/forms";

@Component({
	selector: "createuser",
	templateUrl: "create.user.component.html",
	styleUrls: ['./create.user.component.css']
})
export class CreateUserComponent implements OnInit {
	errorMessage: string;
	userForm: FormGroup;

	public creds = {
		username: "",
		emailaddress: "",
		password: "",
		confirm: ""
	};

	constructor(
		private fb: FormBuilder,
		private router: Router,
		private data: DataService
	) {
		if (!this.data.loginRequired) {
			this.router.navigate([""]);
		}
	}

	ngOnInit(): void {
		this.userForm = this.fb.group(
			{
				username: ["", [Validators.required, Validators.pattern("[a-zA-Z0-9]+")]],
				email: ["", [Validators.required, Validators.pattern("(.+)@(.+){2,}\.(.+){2,}")]],
				password: ["", [Validators.required, Validators.minLength(6)]],
				passwordConfirm: ["", [Validators.required, Validators.minLength(6)]],
			},
			{
				validator: this.compareValidator('password', 'passwordConfirm')
			})
	}

	compareValidator(fc1: string, fc2: string) {
		return (group: FormGroup): { [key: string]: any } => {
			let password = group.controls[fc1];
			let passwordConfirm = group.controls[fc2];
			if (password.value === passwordConfirm.value) {
				return { compareFailed: false };
			}
			return { compareFailed: true }
		}
	}

	onCreateUser() {
		this.data.addUser(this.userForm.value).subscribe((data) => {
			if (data.error == null) {
				this.errorMessage = "";
				this.creds.username = this.userForm.value.username;
				this.creds.emailaddress = this.userForm.value.email;
				this.creds.password = this.userForm.value.password;

				this.data.login(this.creds).subscribe((success) => {
					if (success) {
						this.data.userName = this.creds.username;
					}
				},
					(err) => { this.errorMessage = "Failed to Login"; });

			}
		});
	}
}