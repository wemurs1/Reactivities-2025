import { Box, Button, Paper, Typography } from '@mui/material';
import { useActivities } from '../../../lib/hooks/useActivites';
import { useParams } from 'react-router';
import { useForm } from 'react-hook-form';
import { useEffect } from 'react';
import { zodResolver } from '@hookform/resolvers/zod';
import { activitySchema, ActivitySchema } from '../../../lib/schemas/activitySchema';
import TextInput from '../../../app/shared/components/TextInput';
import SelectInput from '../../../app/shared/components/SelectInput';
import { categoryOptions } from './categoryOptions';
import DateTimeInput from '../../../app/shared/components/DateTimeInput';

export default function ActivityForm() {
  const { reset, handleSubmit, control } = useForm<ActivitySchema>({
    mode: 'onTouched',
    resolver: zodResolver(activitySchema),
  });
  const { id } = useParams();
  const { updateActivity, createActivity, activity, isLoadingActivity } = useActivities(id);

  useEffect(() => {
    if (activity) reset(activity);
  }, [activity, reset]);

  const onSubmit = (data: ActivitySchema) => {
    console.log(data);
  };

  if (isLoadingActivity) return <Typography>Loading activity...</Typography>;

  return (
    <Paper sx={{ borderRadius: 3, padding: 3 }}>
      <Typography variant='h5' gutterBottom color='primary'>
        {activity ? 'Edit' : 'Create'} Activity
      </Typography>
      <Box
        component='form'
        onSubmit={handleSubmit(onSubmit)}
        display='flex'
        flexDirection='column'
        gap={3}
      >
        <TextInput label='Title' control={control} name='title' />
        <TextInput label='Description' control={control} name='description' multiline rows={3} />
        <SelectInput items={categoryOptions} label='Category' control={control} name='category' />
        <DateTimeInput label='Date' control={control} name='date' />
        <TextInput label='City' control={control} name='city' />
        <TextInput label='Venue' control={control} name='venue' />

        <Box display='flex' justifyContent='end' gap={3}>
          <Button color='inherit'>Cancel</Button>
          <Button
            type='submit'
            color='success'
            variant='contained'
            disabled={updateActivity.isPending || createActivity.isPending}
          >
            Submit
          </Button>
        </Box>
      </Box>
    </Paper>
  );
}
