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

    
}