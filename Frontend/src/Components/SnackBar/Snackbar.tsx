import React, { FC, forwardRef, useContext } from 'react';
import { SnackbarContext } from './SnackbarContextProvider';
import { default as MUISnackbar } from '@mui/material/Snackbar';
import MuiAlert, { AlertProps } from '@mui/material/Alert';

const Snackbar: FC = () => {
	const {
		state: { open, severity, message },
		actions: { closeSnackbar },
	} = useContext(SnackbarContext);

	const Alert = forwardRef<HTMLDivElement, AlertProps>(function Alert(props, ref) {
		return <MuiAlert elevation={6} ref={ref} variant="filled" {...props} />;
	});

	return (
		<div>
			<MUISnackbar open={open} anchorOrigin={{ vertical: 'bottom', horizontal: 'left' }} autoHideDuration={4000} onClose={() => closeSnackbar()}>
				<Alert onClose={() => closeSnackbar()} severity={severity} sx={{ width: '100%' }}>
					{message}
				</Alert>
			</MUISnackbar>
		</div>
	);
};

export default Snackbar;
