export interface IsLoading {
	isLoading: true | false;
	error: string | null;
}

export type User = {
	id: number;
	email: string;
	username: string;
};
