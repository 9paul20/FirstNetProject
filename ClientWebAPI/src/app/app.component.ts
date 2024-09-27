import { HttpClient } from "@angular/common/http";
import { Component, inject, OnInit } from "@angular/core";
import { RouterOutlet } from "@angular/router";

@Component({
	selector: "app-root",
	standalone: true,
	imports: [RouterOutlet],
	templateUrl: "./app.component.html",
	styleUrls: ["./app.component.css"],
})
export class AppComponent implements OnInit {
	http = inject(HttpClient);
	title = "DatingApp";
	users: any;

	ngOnInit(): void {
		this.http.get("https://localhost:7079/api/users").subscribe({
			next: (response) => (this.users = response),
			error: (error) => console.error(error),
			complete: () => console.log("Complete"),
		});
	}
}
