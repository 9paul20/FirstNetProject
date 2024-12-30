import { Component, inject, OnInit } from "@angular/core";
import { RouterOutlet } from "@angular/router";
import { NavComponent } from "./nav/nav.component";
import { AccountService } from "./_services/account.service";

@Component({
	selector: "app-root",
	standalone: true,
	imports: [RouterOutlet, NavComponent],
	templateUrl: "./app.component.html",
	styleUrls: ["./app.component.css"],
})
export class AppComponent implements OnInit {
	private AccountService = inject(AccountService);
	users: any;

	setCurrentUser() {
		const userString = localStorage.getItem("user");
		if (!userString) return;
		const user = JSON.parse(userString);
		this.AccountService.currentUser.set(user);
	}

	ngOnInit(): void {
		this.setCurrentUser();
	}
}
