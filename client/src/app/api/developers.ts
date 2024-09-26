const BASE_URL = "http://localhost:3001/api";

export async function getAll() {
    const response = await fetch(`${BASE_URL}/Developers`);
    return response.json();
}

import { Developer } from '../types';

export const GetAllDevelopers = async (): Promise<Developer[]> => {
    const response = await fetch(`${BASE_URL}/developers`);
    if (!response.ok) throw new Error('Failed to fetch developers');
    const data = await response.json();
    return data.developers;
};

export const GetAllDevelopers2 = async (): Promise<Developer[]> => {
    const response = await fetch(`${BASE_URL}/Developers`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
        },
    });
    if (!response.ok) {
        throw new Error('Failed to fetch timelogs');
    }

    const result = await response.json();
    return result.timelogs;
};


export const createDeveloper = async (Developer: Developer): Promise<Developer> => {
    return Developer;
};

export const updateDeveloper = async (Developer: Developer): Promise<void> => {    
    Developer;
};

export const deleteDeveloper = async (DeveloperIds: number[]): Promise<void> => {    
    DeveloperIds;
};
