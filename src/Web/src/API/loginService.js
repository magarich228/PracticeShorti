const data = {
    userName: "ilya",
    password: "12345",
    confirmPassword: "12345"
};

export default class LoginService {
    static async signup() {
        const res = await fetch("http://localhost:5064/api/Identity/signup", {
            method: "POST",
            mode: "cors",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(data)
        });

        console.log(res);
    }
}
