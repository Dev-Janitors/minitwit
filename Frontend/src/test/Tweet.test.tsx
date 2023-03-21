import React from 'react';
import { screen, waitFor } from '@testing-library/react';
import Tweet from '../Components/Tweet/Tweet';
import SnackbarContextProvider from '../Components/SnackBar/SnackbarContextProvider';
import { createRoot } from 'react-dom/client';
import { act } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { Snackbar } from '@mui/material';

let container = {} as Element | DocumentFragment;

beforeEach(() => {
	container = document.createElement('div');
	document.body.appendChild(container);
});

afterEach(() => {
	document.body.removeChild(container as Element);
	container = {} as Element | DocumentFragment;
});

describe('Tweet', () => {
	it('should render', () => {
		expect.assertions(3);
		act(() => {
			createRoot(container).render(
				<SnackbarContextProvider>
					<Tweet />
					<Snackbar />
				</SnackbarContextProvider>
			);
		});

		const button = document.querySelector('button');
		const textArea = document.querySelector('textarea');
		const label = document.querySelector('label');

		expect(button?.textContent).toBe('Tweet');
		expect(textArea?.textContent).toBe('');
		expect(label?.textContent).toBe('Tweet');
	});

	it("shouldn't be able to tweet if tweet is empty", async () => {
		act(() => {
			createRoot(container).render(
				<SnackbarContextProvider>
					<Tweet />
					<Snackbar />
				</SnackbarContextProvider>
			);
		});

		const button = document.querySelector('button');
		const textArea = document.querySelector('textarea');

		expect(button?.textContent).toBe('Tweet');
		expect(textArea?.textContent).toBe('');

		//Click the tweet button
		userEvent.click(button as Element);

		await waitFor(() => {
			expect(screen.getByText('Tweet is empty!')).toBeInTheDocument();
		});
	});

	it('should be able to tweet if tweet is not empty', () => {
		act(() => {
			createRoot(container).render(
				<SnackbarContextProvider>
					<Tweet />
					<Snackbar />
				</SnackbarContextProvider>
			);
		});

		const button = document.querySelector('button');
		const textArea = document.querySelector('textarea');

		expect(button?.textContent).toBe('Tweet');
		expect(textArea?.textContent).toBe('');

		//Type in the text area
		userEvent.type(textArea as Element, 'Hello World');

		//Click the tweet button
		userEvent.click(button as Element);

		screen.findByText('Tweeted successfully!');
	});
});
