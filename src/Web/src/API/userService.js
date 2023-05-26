export default class UserService {
    static async getCur(accessToken) {
        const res = await fetch("http://localhost:5171/api/Users/current", {
            method: "GET",
            mode: "cors",
            headers: {
                "Authorization": accessToken
            }
        });

        return res;
    }
}