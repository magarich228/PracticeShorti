export default class ShortsService {
    static async getShorts(accessToken, page, count) {
        const res = await fetch(`http://localhost:5171/api/Shorts?page=${page}&count=${count}`, {
            method: "GET",
            mode: "cors",
            headers: {
                "Authorization": 'Bearer ' + accessToken
            }
        });

        return res;
    }

    static async getUserShorts(accessToken, userId) {
        const res = await fetch(`http://localhost:5171/api/Shorts/user/${userId}`, {
            method: "GET",
            mode: "cors",
            headers: {
                "Authorization": 'Bearer ' + accessToken
            }
        });

        return res;
    }

    static async getLikedShorts(accessToken, userId) {
        const res = await fetch(`http://localhost:5171/api/Activities/liked-shorts/${userId}`, {
            method: "GET",
            mode: "cors",
            headers: {
                "Authorization": 'Bearer ' + accessToken
            }
        });

        return res;
    }

    static async getSubscriptionShorts(accessToken, userId) {
        const res = await fetch(`http://localhost:5171/api/Activities/subscription-shorts/${userId}`, {
            method: "GET",
            mode: "cors",
            headers: {
                "Authorization": 'Bearer ' + accessToken
            }
        });

        return res;
    }

    static async publicateShort(accessToken, form) {
        const res = await fetch(`http://localhost:5171/api/Shorts`, {
            method: "POST",
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