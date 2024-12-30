import { HttpClient } from "@angular/common/http";
import { Component, inject, OnInit } from "@angular/core";
import { SharedService } from "../../_services/shared.service";

@Component({
	selector: "app-home",
	standalone: true,
	imports: [],
	templateUrl: "./home.component.html",
	styleUrl: "./home.component.css",
})
export class HomeComponent implements OnInit {
	http = inject(HttpClient);
	users: any;

	constructor(public SharedService: SharedService) {}

	ngOnInit(): void {
		this.getUsers();
	}

	getUsers() {
		this.http.get("https://localhost:7079/api/users").subscribe({
			next: (response) => (this.users = response),
			error: (error) => console.error(error),
			complete: () => console.log("Complete"),
		});
	}
}
