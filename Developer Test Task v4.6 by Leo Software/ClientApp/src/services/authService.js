export const login = async (username, password) => {
    try {
        const response = await fetch('/api/auth/login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ username, password }),
        });

        if (!response.ok) {
            throw new Error('Authentication failed');
        }

        const data = await response.json();
        if (data.success) {
            // Store token securely
            sessionStorage.setItem('booksy_token', data.data.accessToken);
            return data.data;
        }
        throw new Error(data.message);
    } catch (error) {
        throw new Error(error.message);
    }
};

export const getStoredToken = () => {
    return sessionStorage.getItem('booksy_token');
};

export const logout = () => {
    sessionStorage.removeItem('booksy_token');
};