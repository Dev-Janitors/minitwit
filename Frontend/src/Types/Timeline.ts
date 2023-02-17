export type Message = {
	id: number;
	authorId: number;
	text: string;
	pubDate: number; // Unix timestamp
	flagged: number;
};
