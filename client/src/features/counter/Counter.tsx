import { Button, ButtonGroup, Typography } from '@mui/material';
import { useStore } from '../../lib/hooks/useStore';
import { observer } from 'mobx-react-lite';

const Counter = observer(function Counter() {
  const { counterStore } = useStore();

  return (
    <>
      <Typography variant='h4' gutterBottom>
        {counterStore.title}
      </Typography>
      <Typography variant='h6'>The count is: {counterStore.count}</Typography>
      <ButtonGroup sx={{ mt: 3 }}>
        <Button onClick={() => counterStore.decrement()} variant='contained' color='error'>
          Decrement
        </Button>
        <Button onClick={() => counterStore.increment()} variant='contained' color='success'>
          Increment
        </Button>
        <Button onClick={() => counterStore.increment(5)} variant='contained' color='primary'>
          Increment by 5
        </Button>
      </ButtonGroup>
    </>
  );
});

export default Counter;
