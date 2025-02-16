import { getStoredToken } from './authService';

export const fetchAppointments = async (startDate, endDate, customerName) => {
    try {
        const token = getStoredToken();
        if (!token) {
            throw new Error('No authentication token found');
        }

        let url = '/api/appointment';
        const params = new URLSearchParams();

        if (startDate) params.append('startDate', startDate.toISOString());
        if (endDate) params.append('endDate', endDate.toISOString());
        if (customerName) params.append('customerName', customerName);

        if (params.toString()) {
            url += `?${params.toString()}`;
        }

        const response = await fetch(url, {
            headers: {
                'Authorization': `Bearer ${token}`,
                'Content-Type': 'application/json',
            },
        });

        if (!response.ok) {
            throw new Error('Failed to fetch appointments');
        }

        const data = await response.json();
        if (data.success) {
            return data.data;
        }
        throw new Error(data.message);
    } catch (error) {
        throw new Error(error.message);
    }
};