import React, { FC, createContext, ReactNode, useState } from 'react';

export type SeverityOptions = 'success' | 'error' | 'warning' | 'info';

interface SnackbarContextValues {
	state: {
		open: boolean;
		severity: SeverityOptions;
		message: string;
	};
	actions: {
		openSnackbar: (severity: SeverityOptions, message: string) => void;
		closeSnackbar: (reason?: string) => void;
	};
}

type Context = SnackbarContextValues;

export const SnackbarContext = createContext<Context>(null as unknown as SnackbarContextValues);

interface Props {
	children: ReactNode;
}

const SnackbarContextProvider: FC<Props> = ({ children }) => {
	const [open, setOpen] = useState(false);
	const [severity, setSeverity] = useState('success' as SeverityOptions);
	const [message, setMessage] = useState('');

	const openSnackbar = (newSeverity: SeverityOptions, newMessage: string) => {
		setSeverity(newSeverity);
		setMessage(newMessage);
		setOpen(true);
	};

	const closeSnackbar = (reason?: string) => {
		if (reason === 'clickaway') {
			return;
		}
		setOpen(false);
	};

	return (
		<SnackbarContext.Provider
			value={{
				state: {
					open,
					severity,
					message,
				},
				actions: {
					openSnackbar,
					closeSnackbar,
				},
			}}
		>
			{children}
		</SnackbarContext.Provider>
	);
};

export default SnackbarContextProvider;
