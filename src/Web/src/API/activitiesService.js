export default class ActivitiesService {
    static async subscribe(accessToken, userId) {
        const res = await fetch(`http://localhost:5171/api/Activities/${userId}`, {
            method: "POST",
            mode: "cors",
            headers: {
                "Authorization": 'Bearer ' + accessToken
            }
        });

        return res;
    } 
    static async unsubsribe(accessToken, userId) {
        const res = await fetch(`http://localhost:5171/api/Activities/${userId}`, {
            method: "DELETE",
            mode: "cors",
            headers: {
                "Authorization": 'Bearer ' + accessToken
            }
        });

        return res;
    }

    static async likeVideo(accessToken, shortId) {
        const res = await fetch(`http://localhost:5171/api/Activities/like/${shortId}`, {
            method: "POST",
            mode: "cors",
            headers: {
                "Authorization": 'Bearer ' + accessToken
            }
        });

        return res;
    }
    static async unlikeVideo(accessToken, shortId) {
        const res = await fetch(`http://localhost:5171/api/Activities/unlike/${shortId}`, {
            method: "DELETE",
            mode: "cors",
            headers: {
                "Authorization": 'Bearer ' + accessToken
            }
        });

        return res;
    }

    static async getUserLikes(accessToken, userId) {
        const res = await fetch(`http://localhost:5171/api/Activities/${userId}/likes`, {
            method: "GET",
            mode: "cors",
            headers: {
                "Authorization": 'Bearer ' + accessToken
            }
        });

        return res;
    }
    static async getUserSubscriptions(accessToken, userId) {
        const res = await fetch(`http://localhost:5171/api/Activities/${userId}/subscriptions`, {
            method: "GET",
            mode: "cors",
            headers: {
                "Authorization": 'Bearer ' + accessToken
            }
        });

        return res;
    }
}