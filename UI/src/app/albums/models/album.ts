import Track from 'src/app/tracks/models/track';

export default interface Album {
  name: string;
  imageUrl: string;
  playCount: number;
  artistName: string | null;
  dateCreated: string;
  description: string | null;
  tracks: Track[];
}
