import { redirect } from '@sveltejs/kit'
import type { Action, Actions, PageServerLoad } from './$types'

export const load:PageServerLoad = async () => {
    // page load
}

//The register action
const login : Action = async({ request }) => {
    //We grab the data we need from the form.
    const data = await request.formData();

    const username = data.get('username');
    const password = data.get('password');

    //Consolidate so that we can pass into the body of the request.
    const credentials = {
        userName: username,
        password: password,
    };

    console.log("things: ", username, password);

    //Attempt to login
    try {
        const response = await fetch('http://localhost:5072/api/Auth/login', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
          },
          body: JSON.stringify(credentials),
        });
  
        if (response.ok) {
            console.log("Logging in: " + username);
        }
    } catch {
        //Handle any errors.
    }

}

export const actions: Actions = { login }
