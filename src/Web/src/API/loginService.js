export default class LoginService {
    static async signup(authData) {
        const res = await fetch("http://localhost:5064/api/Identity/signup", {
            method: "POST",
            mode: "cors",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(authData)
        });

        return res;
    }

    static async signin(authData) {
        const res = await fetch("http://localhost:5064/api/Identity/signin", {
            method: "POST",
            mode: "cors",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(authData)
        });

        return res;
    }

    static async logout(accessToken) {
        const res = await fetch("http://localhost:5171/api/Identity/logout", {
            method: "DELETE",
            mode: "cors",
            headers: {
                "Authorization": accessToken
            }
        });

        return res;
    }

    static async refreshToken(refreshToken, accessToken) {
        const res = await fetch("http://localhost:5064/api/Identity/refresh-token", {
            method: "POST",
            mode: "cors",
            headers: {
                "Authorization": accessToken,
                "Content-Type": "application/json"
            },
            body: JSON.stringify({refreshToken})
        });

        return res;
    }
}
