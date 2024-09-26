const BASE_URL = "http://localhost:3001/api";

import { Project } from '../types';

export const getAllProjects = async (developerId: number): Promise<Project[]> => {
    const response = await fetch(`${BASE_URL}/projects/getall`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ developerId }),
    });
    if (!response.ok) {
        throw new Error('Failed to fetch timelogs');
    }
    const data = await response.json();
    return data.projects;
};

export const createProject = async (project: Project): Promise<void> => {
    const response = await fetch(`${BASE_URL}/projects`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(project),
    });
    if (!response.ok) throw new Error('Failed to create project');
};

export const updateProject = async (project: Project): Promise<void> => {    
    const response = await fetch(`${BASE_URL}/projects/updateproject`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({
            id: project.id,
            name: project.name,
            developerId: project.developerId,
            customerId: project.customerId,
            deadline: project.deadline,
            isFinished: project.isFinished
        })
    });
    if (!response.ok) throw new Error('Failed to update project');
};

export const deleteProject = async (projectIds: number[]): Promise<void> => {
    const response = await fetch(`${BASE_URL}/projects`, {
        method: 'DELETE',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ projectIds }),
    });
    if (!response.ok) throw new Error('Failed to delete project');
};

export const finishProjects = async (projectIds: number[]): Promise<void> => {
    const response = await fetch(`${BASE_URL}/projects/finishProjects`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ projectIds }),
    });
    if (!response.ok) throw new Error('Failed to finish projects');
};

