const BASE_URL = "http://localhost:3001/api";

import { Timelog } from '../types';

export const fetchTimelogs = async (): Promise<Timelog[]> => {
    const response = await fetch(`${BASE_URL}/timelogs`);
    if (!response.ok) {
        throw new Error('Failed to fetch timelogs');
    }
    return response.json();
};

export const GetAllTimelogs = async (developerId: number): Promise<Timelog[]> => {
    const response = await fetch(`${BASE_URL}/timelogs/getall`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ developerId }),
    });
    if (!response.ok) {
        throw new Error('Failed to fetch timelogs');
    }

    const result = await response.json();
    return result.timelogs;
};

export const createTimelog = async (timelog: Timelog): Promise<void> => {
    const response = await fetch(`${BASE_URL}/timelogs`, {
        method: 'POST',       
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(timelog)
    });
    if (!response.ok) throw new Error('Failed to create timelog');
};

export const updateTimelog = async (timelog: Timelog): Promise<void> => {
    const response = await fetch(`${BASE_URL}/timelogs/`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
        },        
        body: JSON.stringify({
            id: timelog.id,
            developerId: timelog.developerId,
            projectId: timelog.projectId,
            timeInMinutes: timelog.timeInMinutes,
        }),
    });
    if (!response.ok) throw new Error('Failed to update timelog');
};


export const deleteTimelog = async (timelogIds: number[]): Promise<void> => {    
    const response = await fetch(`${BASE_URL}/timelogs/`, {
        method: 'DELETE',
        headers: {
            'Content-Type': 'application/json',
        },        
        body: JSON.stringify({
            timelogIds,
        }),
    });
    if (!response.ok) throw new Error('Failed to update timelog');
};

