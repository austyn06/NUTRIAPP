// @ts-nocheck

import type { Actions, PageServerLoad } from "./$types"

export const load: PageServerLoad = async ({ locals }) => {
	// Get team info
	let team = {
		name: "",
		members: [],
		challengeStartDate: new Date(),
		challengeEndDate: new Date(),
	};

	if (!locals.user) return { user: {}, team: team };

	const response = await fetch("http://localhost:5072/api/teams", {
		method: "GET",
		headers: { "sessionKey": locals.user.session_key }
	});

	if (response.ok) {
		const responseData = await response.json();
		team = {
			name: responseData.name,
			members: responseData.members,
			challengeStartDate: new Date(responseData.challengeStartDate),
			challengeEndDate: new Date(responseData.challengeEndDate)
		}
	}
	else {
		console.log(`${response.status} : ${response.statusText}`)
	}

	return {
		user: locals.user,
		team: team
	};
}