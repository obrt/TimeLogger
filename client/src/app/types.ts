export interface Project {
    id: number;
    name: string;
    developerName: string;
    developerId: number;
    customerName: string;
    customerId: number;
    totalTimeLogged: number;
    deadline: string;
    isFinished: boolean;
}

export interface Customer {
    id: number;
    name: string;
    developerName: string;
    developerId: number;
}

export interface Timelog {
    id: number;
    projectName: string;
    projectId: number;
    developerName: string;
    developerId: number;
    timeInMinutes: number;
}

export interface Developer {
    id: number;
    firstName: string;
    lastName: string;
    email: string;
    username: string;
    password: string;
}