package files;

import exception.FileNameAlreadyExistsException;

import java.util.ArrayList;
import java.util.List;

public class FileStorage {
    private ArrayList<File> files = new ArrayList<>();
    private double availableSize = 100;
    private double maxSize = 100;

    /**
     * Construct object and set max storage size and available size according passed values
     * @param size files.FileStorage size
     */
    public FileStorage(int size) {
        maxSize = size;
        availableSize += maxSize;
    }

    /**
     * Construct object and set max storage size and available size based on default value=100
     */
    public FileStorage() {
    }

    /**
     * Write file in storage if filename is unique and size is not more than available size
     * @param file to save in file storage
     * @return result of file saving
     * @throws FileNameAlreadyExistsException in case of already existent filename
     */
    public boolean write(File file) throws FileNameAlreadyExistsException {
        if (isExists(file.getFilename())) {
            throw new FileNameAlreadyExistsException();
        }
        if (file.getSize() >= availableSize) {
            return false;
        }
        files.add(file);
        availableSize -= file.getSize();

        return true;
    }

    /**
     * Check is file exist in storage
     * @param fileName to search
     * @return result of checking
     */
    public boolean isExists(String fileName) {
        for (File file: files) {
            if (file.getFilename().contains(fileName)) {
                return true;
            }
        }
        return false;
    }

    /**
     * Delete file from storage
     * @param fileName of file to delete
     * @return result of file deleting
     */
    public boolean delete(String fileName) {
        return files.remove(getFile(fileName));
    }

    /**
     * Get all Files saved in the storage
     * @return list of files
     */
    public List<File> getFiles() {
        return files;
    }

    /**
     * Get file by filename
     * @param fileName of file to get
     * @return file
     */
    public File getFile(String fileName) {
        if (isExists(fileName)) {
            for (File file: files) {
                if (file.getFilename().contains(fileName)) {
                    return file;
                }
            }
        }
        return null;
    }

    /**
     * Remove all files
     */
    public void deleteAllFiles() {
        files.removeAll(files);
    }
}
