export type ValidationRule = {
    name: string;
    rule: (value: string) => boolean;
    message: string;
}
type Validator = (data: FormData) => string | null;
export function validationFormFactory(rules: ValidationRule[]): Validator {
    const validateForm = (data: FormData): string | null => {
        for(const [key, item] of data) {
            const currentRule = rules.find(item => item.name == key);
            if (currentRule == undefined || typeof(item) != 'string') continue;
            const { message, rule } = currentRule;
            if(!rule(item)) return message;
        }        
        return null;
    }
    return validateForm;
}