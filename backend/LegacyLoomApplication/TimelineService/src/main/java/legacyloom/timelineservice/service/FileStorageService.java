package legacyloom.timelineservice.service;

import org.springframework.web.multipart.MultipartFile;

import java.util.List;

public interface FileStorageService {
    List<String> uploadFiles(List<MultipartFile> files, String path);
    void deleteFile(String filePath);
}