import { AccountService } from "./../../_services/account.service";
import { Component } from "@angular/core";
import { FormsModule } from "@angular/forms";

@Component({
	selector: "app-login",
	standalone: true,
	imports: [FormsModule],
	templateUrl: "./login.component.html",
	styleUrl: "./login.component.css",
})
export class LoginComponent {
	model: any = {};
	loggedIn?: boolean;

	constructor(public AccountService: AccountService) {}

	login() {
		this.AccountService.login(this.model).subscribe({
			next: (response) => {
				console.log(response);
			},
			error: (error) => console.error(error),
			complete: () => console.log("Complete"),
		});
	}
}
