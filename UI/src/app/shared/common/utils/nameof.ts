export const nameof = <T>(propertyName: Extract<keyof T, string>): Extract<keyof T, string> => propertyName;
