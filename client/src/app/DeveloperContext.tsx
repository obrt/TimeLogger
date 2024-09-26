import React, { createContext, useContext, useState, ReactNode } from "react";

// Define the shape of the context
interface DeveloperContextType {
    developerId: number | null;
    setDeveloperId: (id: number) => void;
}

// Create the context with default values
const DeveloperContext = createContext<DeveloperContextType | undefined>(undefined);

// Export a custom hook for consuming the context
export const useDeveloper = () => {
    const context = useContext(DeveloperContext);
    if (!context) {
        throw new Error("useDeveloper must be used within a DeveloperProvider");
    }
    return context;
};

// Provider component
export const DeveloperProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
    const [developerId, setDeveloperId] = useState<number | null>(null); // Developer ID state

    return (
        <DeveloperContext.Provider value={{ developerId, setDeveloperId }}>
            {children}
        </DeveloperContext.Provider>
    );
};
