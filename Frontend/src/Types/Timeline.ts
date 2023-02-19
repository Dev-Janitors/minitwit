export type Message = {
	id?: number;
	authorId?: number;
	content: string;
	user: string;
	pubDate: number; // Unix timestamp
	flagged?: number;
};
