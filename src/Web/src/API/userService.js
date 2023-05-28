export default class UserService {
    static async getCur(accessToken) {
        const res = await fetch("http://localhost:5171/api/Users/current", {
            method: "GET",
            mode: "cors",
            headers: {
                "Authorization": 'Bearer ' + accessToken
            }
        });

        return res;
    }

    static async getById(accessToken, userId) {
        const res = await fetch(`http://localhost:5171/api/Users/${userId}`, {
            method: "GET",
            mode: "cors",
            headers: {
                "Authorization": 'Bearer ' + accessToken
            }
        });

        return res;
    }

    static async avatarUpdate(accessToken, form) {
        const res = await fetch("http://localhost:5171/api/Users/avatar-update", {
            method: "PUT",
            mode: "cors",
            headers: {
                "Authorization": 'Bearer ' + accessToken,
                // "Content-Type": "multipart/form-data"
            },
            body: new FormData(form)
        });

        return res;
    }
}