import { MapContainer, TileLayer, Marker, Popup } from 'react-leaflet';
import { Icon } from 'leaflet';
import markerIconPng from 'leaflet/dist/images/marker-icon.png';
import 'leaflet/dist/leaflet.css';

type Props = {
  position: [number, number];
  venue: string;
};

export default function MapComponent({ position, venue }: Props) {
  return (
    <MapContainer center={position} zoom={13} scrollWheelZoom={false} style={{ height: '100%' }}>
      <TileLayer url='https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png' />
      <Marker position={position} icon={new Icon({ iconUrl: markerIconPng })}>
        <Popup>{venue}</Popup>
      </Marker>
    </MapContainer>
  );
}
