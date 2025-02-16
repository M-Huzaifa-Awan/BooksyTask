import React, { useState, useEffect } from 'react';
import { fetchAppointments } from '@/services/appointmentService';
import AppointmentCard from './AppointmentCard';

const CalendarView = () => {
    const [appointments, setAppointments] = useState([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);
    const [filters, setFilters] = useState({
        startDate: null,
        endDate: null,
        customerName: '',
    });

    useEffect(() => {
        loadAppointments();
    }, [filters]);

    const loadAppointments = async () => {
        try {
            setLoading(true);
            const data = await fetchAppointments(
                filters.startDate,
                filters.endDate,
                filters.customerName
            );
            setAppointments(data);
            setError(null);
        } catch (err) {
            alert(err.message);
        } finally {
            setLoading(false);
        }
    };

    const handleFilterChange = (name, value) => {
        setFilters(prev => ({
            ...prev,
            [name]: value
        }));
    };

    if (loading) {
        return <div className="flex justify-center items-center h-64">Loading...</div>;
    }

    return (
        <div className="p-4">
            <div className="mb-4 space-y-4">
                <div className="flex gap-4">
                    <input
                        type="date"
                        className="border rounded p-2"
                        onChange={(e) => handleFilterChange('startDate', new Date(e.target.value))}
                    />
                    <input
                        type="date"
                        className="border rounded p-2"
                        onChange={(e) => handleFilterChange('endDate', new Date(e.target.value))}
                    />
                    <input
                        type="text"
                        placeholder="Search by customer name"
                        className="border rounded p-2"
                        onChange={(e) => handleFilterChange('customerName', e.target.value)}
                    />
                </div>
            </div>

            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
                {appointments.map((appointment) => (
                    <AppointmentCard
                        key={appointment.appointmentUid}
                        appointment={appointment}
                    />
                ))}
            </div>
        </div>
    );
};

export default CalendarView;